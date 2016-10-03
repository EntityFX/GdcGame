angular
    .module("gdCameApp")
    .factory("gdCameApiService",
        function($rootScope, $http, $location) {
            var gameApiServiceBaseUri = $location.protocol() + "://" + $location.host() + ":" + 8889 + "/api/game/";

            var gdCameApiServiceFactory = {
                getGameData: function() {
                    return $http.get(gameApiServiceBaseUri + "game-data/")
                        .then(function(result) {
                            return result;
                        });
                },

                getCounters: function() {
                    return $http.get(gameApiServiceBaseUri + "counters/")
                        .then(function(result) {
                            return result;
                        });
                },

                performManualStep: function(data) {
                    return $http.post(gameApiServiceBaseUri + "perform-step/", data)
                        .then(function(result) {
                            return result;
                        });
                },

                buyFundDriver: function(fundDriverId) {
                    return $http.post(gameApiServiceBaseUri + "buy-item/", fundDriverId)
                        .then(function(result) {
                            return result;
                        });
                },

                fightAgainstInflation: function() {
                    $http.post(gameApiServiceBaseUri + "fight-inflation/");
                },

                activateDelayedCounter: function(counterId) {
                    $http.post(gameApiServiceBaseUri + "ActivateDelayedCounter/", counterId);
                }
            };
            return gdCameApiServiceFactory;
        });