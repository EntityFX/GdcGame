angular.module("gdCameApp").controller("GameController",
[
    "$rootScope", "$scope", "$interval", "gameData", "gdCameApiService", "$location",
    function ($rootScope, $scope, $interval, gameData, gdCameApiService, $location) {

        function calculateGameCounters() {
            $rootScope.gameData.cash.onHand += $rootScope.gameData.cash.counters[2].value;
        }

        var gameCalculateInterval = $interval(calculateGameCounters, 1000);

        $scope.$on("$destroy", function () {
            $interval.cancel(gameCalculateInterval);
        });


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
            $scope.isTakeRestDisabled = isTakeRestActive();
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
            gdCameApiService.activateDelayedCounter($rootScope.gameData.cash.counters[3].id);
        };

        function isTakeRestActive() {
            return $rootScope.gameData.cash.counters[2].inflation == 0;
        }

        function isDoQuarterGoalActive() {
            return $rootScope.gameData.cash.counters[3].unlockValue > $rootScope.gameData.cash.counters[0].value
                || $rootScope.gameData.cash.counters[3].secondsRemaining > 0;
        }
    }
]);