angular
    .module("gdCameApp")
    .component("counters",
    {
        templateUrl: "/app/templates/counters.template.html",
        controller: function CountersController($rootScope, $scope) {
            $rootScope.$watch("gameData",
                function () {
                    if ($rootScope.gameData !== undefined)
                        $scope.counters = $rootScope.gameData.Counters;
                });


            $scope.$on('counters.update',
                function (event, value) {
                    $scope.counters.TotalFunds = value.TotalFunds;
                    $scope.counters.CurrentFunds = value.CurrentFunds;

                    value.Counters.forEach(function (item) {
                        var oldCounter = $rootScope.gameData.Counters.Counters.filter(function (counter) {
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
    });
