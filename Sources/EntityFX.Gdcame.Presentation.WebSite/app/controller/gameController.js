angular.module("gdCameApp").controller("GameController",
[
    "$rootScope", "$scope", "$interval", "gameData", "gdCameApiService", "$location",
    function ($rootScope, $scope, $interval, gameData, gdCameApiService, $location) {
        //$.connection.hub.url = $location.protocol() + "://" + $location.host() + ":8080/signalr";
        $scope.isTakeRestDisabled = true;
        $scope.isDoQuarterGoalDisabled = true;
        /*var hub = $.connection.gameDataHub;
        if (hub != null) {
            hub.client.getGameData = function(data) {

                $rootScope.$apply(function() {
                        $rootScope.gameData = data;
                        $scope.isTakeRestDisabled = isTakeRestActive();
                        $scope.isDoQuarterGoalDisabled = isDoQuarterGoalActive();
                    }
                );
                $scope.$broadcast("update.fundsDrivers", $rootScope.gameData);
            };
            $.connection.hub.start();

        }*/

        gameData.then(function (data) {
            $rootScope.gameData = data;
        });

        $scope.performManualStep = function () {
            gdCameApiService.performManualStep($scope.verificationNumber)
                .then(function (value) {
                    if (value.data.modifiedCountersInfo != undefined) {

                        if (value.data.modifiedCountersInfo != undefined) {
                            $rootScope.$broadcast("counters.update", value.data.modifiedCountersInfo);
                        }
                    }

                    $scope.verificationData = value.data.verificationData;

                    if ($scope.verificationData != null) {
                        $scope.visibility = true;
                        $scope.verificationNumber = null;
                    } else {
                        $scope.visibility = false;
                    }

                });
        };
        $scope.$on("counters.update",
            function (event, value) {
                $scope.isTakeRestDisabled = isTakeRestActive();
            });

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
        };
        $scope.activateDelayedCounter = function () {
            gdCameApiService.activateDelayedCounter($rootScope.gameData.cash.counters[3].Id);
        };

        function isTakeRestActive() {
            return $rootScope.gameData.cash.counters[2].Inflation == 0;
        }

        function isDoQuarterGoalActive() {
            return $rootScope.gameData.cash.counters[3].UnlockValue > $rootScope.gameData.cash.counters[0].Value
                || $rootScope.gameData.cash.counters[3].SecondsRemaining > 0;
        }
    }
]);