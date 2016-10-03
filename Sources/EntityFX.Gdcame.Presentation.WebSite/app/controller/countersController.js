angular.module("gdCameApp").controller('CountersController',
    [
        "$rootScope", "$scope", function ($rootScope, $scope) {
            $rootScope.$watch("gameData",
                function () {
                    if ($rootScope.gameData !== undefined)
                        $scope.cash = $rootScope.gameData.cash;
                });


            $scope.$on("counters.update",
                function (event, value) {
                    $scope.cash.totalEarned = value.totalEarned;
                    $scope.cash.onHand = value.onHand;

                    value.counters.forEach(function (item) {
                        var oldCounter = $rootScope.gameData.cash.counters.filter(function (counter) {
                            return counter.id === item.id;
                        })[0];

                        if (oldCounter != undefined) {

                            oldCounter.value = item.value;
                            if (item.type = 1) {
                                oldCounter.subValue = item.subValue;
                                oldCounter.bonusPercentage = item.bonusPercentage;
                                oldCounter.bonus = item.bonus;
                                oldCounter.inflation = item.inflation;
                            }
                            if (item.type = 2) {
                                oldCounter.unlockValue = item.unlockValue;
                            }
                        }
                    });
                });
        }
    ]);