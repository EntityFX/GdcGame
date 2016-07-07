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
                    $http.post(gameApiServiceBaseUri + "PerformManualStep/");
                },

                buyFundDriver: function (fundDriverId) {
                    $http.post(gameApiServiceBaseUri + "BuyFundDriver/", fundDriverId);
                },                
                
                fightAgainstInflation: function () {
                    $http.post(gameApiServiceBaseUri + "FightAgainstInflation/");
                }
            }

            return gdCameApiServiceFactory;
        });