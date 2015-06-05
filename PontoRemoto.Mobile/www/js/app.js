var mblitApp = angular.module('mblitApp', ['ngRoute', 'ngAnimate', 'ngSanitize', 'ngCordova', 'mobileit.directives', 'mblitApp.controllers']);

mblitApp.run(function($rootScope, setup) {
	setup.initialize(
		{
			// server: "http://localhost:7112", // Localhost
			server: "http://www.mbl-it.com/PontoRemoto", // Staging

			apiversion: "V1",

			firstPage: "/checkin",
			
			encryptionInfo: {
								password: "IcnP12msKQrzfBcrJ",
								salt: "PontoRemoto",
								passwordIterations: 2,
								initialVector: "DJBar03x#hqo50vL",
								keySize: 256 / 32
							}
		}
	);

	waitDomReady(function() {
		var checkins = localStorage.getItem("checkins");
		
		$rootScope.checkins = checkins ? JSON.parse(checkins) : { pending: [], saved: [] };
	});
});