angular
    .module("gdCameApp")
    .factory("gdCameApiService",
        function ($rootScope, $http, $location) {
            var gameApiServiceBaseUri = $location.protocol() + "://" + $location.host() + ":" + $location.port() + "/api/GameApi/";

            var gdCameApiServiceFactory = {
                getGameData: function () {
                    return $http.get(gameApiServiceBaseUri + "GetGameData/")
                        .then(function (result) {
                            return result;
                        });
                },

                getCounters: function () {
                    return $http.get(gameApiServiceBaseUri + "GetCounters/")
                        .then(function (result) {
                            return result;
                        });
                },

                performManualStep: function (data) {
                    return $http.post(gameApiServiceBaseUri + "PerformManualStep/", data)
                        .then(function (result) {
                            return result;
                        });
                },

                buyFundDriver: function (fundDriverId) {
                    return $http.post(gameApiServiceBaseUri + "BuyFundDriver/", fundDriverId)
                        .then(function (result) {
                            return result;
                        });
                },

                fightAgainstInflation: function () {
                    $http.post(gameApiServiceBaseUri + "FightAgainstInflation/");
                },

                activateDelayedCounter: function (counterId) {
                    $http.post(gameApiServiceBaseUri + "ActivateDelayedCounter/", counterId);
                }
            }

            return gdCameApiServiceFactory;
        });