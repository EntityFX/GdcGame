angular
    .module("gdCameApp")
    .factory("gdCameApiService",
        function ($rootScope, $http, $location, $localStorage, apiServiceUri) {
            var gdCameApiServiceFactory = {
                getGameData: function() {
                    return $http.get($localStorage.globals.apiAddress + "/api/game/game-data/")
                        .then(function(result) {
                            return result;
                        });
                },

                getCounters: function() {
                    return $http.get($localStorage.globals.apiAddress + "/api/game/counters/")
                        .then(function(result) {
                            return result;
                        });
                },

                performManualStep: function(data) {
                    return $http.post($localStorage.globals.apiAddress + "/api/game/perform-step/", data)
                        .then(function(result) {
                            return result;
                        });
                },

                buyFundDriver: function(fundDriverId) {
                    return $http.post($localStorage.globals.apiAddress + "/api/game/buy-item/", fundDriverId)
                        .then(function(result) {
                            return result;
                        });
                },

                fightAgainstInflation: function() {
                    return $http.post($localStorage.globals.apiAddress + "/api/game/fight-inflation/").then(function (result) {
                        return result;
                    });
                },

                activateDelayedCounter: function(counterId) {
                    return $http.post($localStorage.globals.apiAddress + "/api/game/ActivateDelayedCounter/", counterId).then(function (result) {
                        return result;
                    });
                }
            };
            return gdCameApiServiceFactory;
        });