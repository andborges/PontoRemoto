mblitApp.config(function($routeProvider) {
	$routeProvider
	.when('/', {
		templateUrl : 'pages/home.html',
		controller  : 'HomeCtrl'
	})
	.when('/error', {
		templateUrl : 'pages/error.html',
		controller  : 'ErrorCtrl'
	})
	.when('/client', {
		templateUrl : 'pages/client.html',
		controller  : 'ClientCtrl'
	})
	.when('/waitapproval', {
		templateUrl : 'pages/waitapproval.html',
		controller  : 'WaitApprovalCtrl'
	})
	.when('/worker', {
		templateUrl : 'pages/worker.html',
		controller  : 'WorkerCtrl'
	})
	.when('/checkin', {
		templateUrl : 'pages/checkin.html',
		controller  : 'CheckinCtrl'
	})
	.when('/checkins', {
		templateUrl : 'pages/checkins.html',
		controller  : 'CheckinsCtrl'
	})
});