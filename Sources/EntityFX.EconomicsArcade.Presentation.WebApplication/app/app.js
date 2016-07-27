var app = angular.module("gdCameApp", []);

app.factory('gameData',
    function ($q, gdCameApiService) {
        var deferred = $q.defer();
        gdCameApiService.getGameData().then(function (value) {
            deferred.resolve(value.data);
        });
        return deferred.promise;
    });

app.controller('appGameController',
[
    '$rootScope', '$scope', '$interval', 'gameData', 'gdCameApiService', '$location',
    function ($rootScope, $scope, $interval, gameData, gdCameApiService, $location) {
        $.connection.hub.url = $location.protocol() + "://" + $location.host() + ":8080/signalr";
        $scope.isTakeRestDisabled = true;
        $scope.isDoQuarterGoalDisabled = true;
        var hub = $.connection.gameDataHub;
        if (hub != null) {
            hub.client.getGameData = function (data) {

                $rootScope.$apply(function () {
                        $rootScope.gameData = data;
                        $scope.isTakeRestDisabled = isTakeRestActive();
                        $scope.isDoQuarterGoalDisabled = isDoQuarterGoalActive();
                    }
                );
                $scope.$broadcast('update.fundsDrivers', $rootScope.gameData);
            };
            $.connection.hub.start();

        }

        gameData.then(function (data) {
            $rootScope.gameData = data;
        });

        $scope.performManualStep = function () {
            gdCameApiService.performManualStep($scope.verificationNumber)
                .then(function (value) {
                    if (value.data.ModifiedCountersInfo != undefined) {

                        if (value.data.ModifiedCountersInfo != undefined) {
                            $rootScope.$broadcast('counters.update', value.data.ModifiedCountersInfo);
                        }
                    }

                    $scope.VerificationData = value.data.VerificationData;

                    if ($scope.VerificationData != null) {
                        $scope.visibility = true;
                        $scope.verificationNumber = null;
                    } else {
                        $scope.visibility = false;
                    }

                });
        }

        $scope.$on('counters.update',
            function (event, value) {
                $scope.isTakeRestDisabled = isTakeRestActive();
            });

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
        }

        $scope.activateDelayedCounter = function () {
            gdCameApiService.activateDelayedCounter($rootScope.gameData.Counters.Counters[3].Id);
        }

        function isTakeRestActive() {
            return $rootScope.gameData.Counters.Counters[2].Inflation == 0;
        }

        function isDoQuarterGoalActive() {
            return $rootScope.gameData.Counters.Counters[3].UnlockValue > $rootScope.gameData.Counters.Counters[0].Value
                || $rootScope.gameData.Counters.Counters[3].SecondsRemaining > 0;
        }
    }]);