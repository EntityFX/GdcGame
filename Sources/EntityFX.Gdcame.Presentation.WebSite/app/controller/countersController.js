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
                    $scope.cash.price = value.price;

                    value.counters.forEach(function (item) {
                        var oldCounter = $rootScope.gameData.cash.counters.filter(function (counter) {
                            return counter.Id === item.Id;
                        })[0];

                        if (oldCounter != undefined) {

                            oldCounter.Value = item.Value;
                            if (item.Type = 1) {
                                oldCounter.SubValue = item.SubValue;
                                oldCounter.BonusPercentage = item.BonusPercentage;
                                oldCounter.Bonus = item.Bonus;
                                oldCounter.Inflation = item.Inflation;
                            }
                            if (item.Type = 2) {
                                oldCounter.UnlockValue = item.UnlockValue;
                            }
                        }
                    });
                });
        }
    ]);