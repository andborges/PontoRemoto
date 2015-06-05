'use strict';

angular.module('mblitApp.controllers', ['ngSanitize', 'mobileit.directives'])
    .controller('HomeCtrl', ['$scope', '$rootScope', 'api', function ($scope, $rootScope, api) {
		if (localStorage.getItem("checkinAvailable")) {
			$rootScope.mbl.screen.go("/checkin");			
			return;
		}

		var clientId = localStorage.getItem("clientId");

		if (!clientId) {
			$rootScope.mbl.screen.go("/client");
			return;
		}

		api.get("Worker", "clientId=" + clientId + "&deviceId=" + window.device.uuid, {},
		function(result) {
			if (!result) {
				$rootScope.mbl.screen.go("/worker");
				return;
			} else if (result.Status == "New" || result.Status == "Revoked") {
				$rootScope.mbl.screen.go("/waitapproval");
				return;
			} else if (result.Status == "Granted") {
				$rootScope.mbl.screen.go("/checkin");
				return;
			}
			
			$rootScope.mbl.screen.go("/error");
		},
		function(message) {
			navigator.notification.alert(message);
		});
    }])
	.controller('ErrorCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
		$scope.refresh = function() {
			$rootScope.mbl.screen.go("/");
		};
		
		waitDomReady(function() {
			$rootScope.mbl.page.addScroll();
		});
	}])
	.controller('ClientCtrl', ['$scope', '$rootScope', 'api', function ($scope, $rootScope, api) {
		$scope.client = {};
		$scope.client.identification = localStorage.getItem("clientIdentification");

		$scope.saveClient = function() {
			api.get("Client", "identification=" + $scope.client.identification, {}, function(result) {
				if (result) {
					localStorage.removeItem("checkinAvailable");
					localStorage.setItem("clientId", result.Id);
					localStorage.setItem("clientIdentification", result.Identification);
					localStorage.setItem("clientName", result.Name);
					localStorage.setItem("workerIdentificationLabel", result.WorkerIdentificationLabel);

					$rootScope.mbl.screen.go("/");
				} else {
					navigator.notification.alert("Código inválido");
				}
			},
			function(message) {
				navigator.notification.alert(message);
			});
		};
		
		waitDomReady(function() {
			$rootScope.mbl.page.addScroll();
		});
	}])
	.controller('WaitApprovalCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
		$scope.clientName = localStorage.getItem("clientName");

		$scope.refresh = function() {
			$rootScope.mbl.screen.go("/");
		};
		
		waitDomReady(function() {
			$rootScope.mbl.page.addScroll();
		});
	}])
	.controller('WorkerCtrl', ['$scope', '$rootScope', 'api', function ($scope, $rootScope, api) {
		$scope.clientName = localStorage.getItem("clientName");
		$scope.workerIdentificationLabel = localStorage.getItem("workerIdentificationLabel");
		$scope.createWorkerViewModel = {};

		$scope.createWorker = function() {
			var clientId = localStorage.getItem("clientId");
			$scope.createWorkerViewModel.clientId = clientId;
			$scope.createWorkerViewModel.deviceId = window.device.uuid;
			$scope.createWorkerViewModel.deviceModel = window.device.model ? window.device.model : "Generic";
			$scope.createWorkerViewModel.devicePlatform = window.device.platform;

			api.post("Worker", $scope.createWorkerViewModel, {},
			function(data) {
				$rootScope.mbl.screen.go("/");
			},
			function(message) {
				navigator.notification.alert(message);
			});
		};
		
		waitDomReady(function() {
			$rootScope.mbl.page.addScroll();
		});
	}])
	.controller('CheckinCtrl', ['$scope', '$rootScope', 'api', '$cordovaNetwork', '$cordovaGeolocation', function ($scope, $rootScope, api, $cordovaNetwork, $cordovaGeolocation) {
		$scope.clientName = localStorage.getItem("clientName");
		localStorage.setItem("checkinAvailable", true);

		$scope.gpsRefresh = function() {
			// $scope.checkinViewModel.latitude = 22;
			// $scope.checkinViewModel.longitude = 23;
			// return;

			$rootScope.mbl.loader.beginLoading();

			var posOptions = { enableHighAccuracy: true, timeout: 30000, maximumAge: 3000 };
			
			$cordovaGeolocation.getCurrentPosition(posOptions)
			.then(function(pos) {
				console.log("GPS success");

				$rootScope.latitude = pos.coords.latitude;
				$rootScope.longitude = pos.coords.longitude;

				$rootScope.mbl.loader.stopLoading();

				waitDomReady(function() {
					$rootScope.mbl.page.scroll.refresh();
				});
			},
			function(error) {
				console.log("GPS error: " + error);

				$scope.enableGpsRefresh = true;
				$rootScope.mbl.loader.stopLoading();
				
				waitDomReady(function() {
					$rootScope.mbl.page.scroll.refresh();
				});
			});
		};

		$scope.checkinArriving = function() {
			$scope.checkinViewModel = { type: "Arriving" };
			saveCheckinOnServer();
		};
		
		$scope.checkinLeaving = function() {
			$scope.checkinViewModel = { type: "Leaving" };
			saveCheckinOnServer();
		};
		
		function saveCheckinOnServer() {
			$scope.checkinViewModel.clientId = localStorage.getItem("clientId");
			$scope.checkinViewModel.deviceId = window.device.uuid;
			$scope.checkinViewModel.date = new Date().format("dd/mm/yyyy HH:MM:ss");
			$scope.checkinViewModel.latitude = $rootScope.latitude;
			$scope.checkinViewModel.longitude = $rootScope.longitude;
		
			api.post("Checkin", $scope.checkinViewModel, {},
			function(data) {
				saveCheckinOnDevice(true);
				$rootScope.mbl.app.toast("Ponto registrado no servidor com sucesso");
			},
			function(message) {
				saveCheckinOnDevice(false);
				
				$rootScope.mbl.app.toast("Ponto registrado no dispositivo: " + message + ". Você poderá registrar o ponto no servidor acessando a tela 'Registros'");
			});
		}
		
		function saveCheckinOnDevice(savedOnServer) {
			if (savedOnServer) {
				$rootScope.checkins.saved.unshift($scope.checkinViewModel);
			} else {
				$rootScope.checkins.pending.unshift($scope.checkinViewModel);
			}
			
			localStorage.setItem("checkins", JSON.stringify($rootScope.checkins));
		}
		
		waitDomReady(function() {
			if (!$rootScope.latitude || !$rootScope.longitude) {
				$scope.gpsRefresh();
			}

			$rootScope.mbl.page.addScroll();
		});
	}])
	.controller('CheckinsCtrl', ['$scope', '$rootScope', function ($scope, $rootScope) {
		$scope.clientName = localStorage.getItem("clientName");
		
		$scope.clearPending = function() {
			$rootScope.checkins.pending = [];
			localStorage.setItem("checkins", JSON.stringify($rootScope.checkins));
		};
		
		$scope.clearSaved = function() {
			$rootScope.checkins.saved = [];
			localStorage.setItem("checkins", JSON.stringify($rootScope.checkins));
		};
		
		waitDomReady(function() {
			$rootScope.mbl.page.addScroll();
		});
	}]);

function checkinTypeDescription(checkinType) {
	if (checkinType == "Arriving")
		return "Entrada";

	if (checkinType == "Leaving")
		return "Saída";
}