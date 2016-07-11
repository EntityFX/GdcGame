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

                performManualStep: function () {
                    return $http.post(gameApiServiceBaseUri + "PerformManualStep/")
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
                }
            }

            return gdCameApiServiceFactory;
        });