/// <reference path="../shared/ControllerBase.ts" />
/// <reference path="../types.d.ts" />
/// <reference path="../../Scripts/typings/angularjs/angular.d.ts" />
namespace Gdcame.Controllers {
    import Cash = Services.Cash;

    export class CountersController extends Controllers.ControllerBase {
        public cash: Cash; 
    }

    angular.module("gdCameApp").controller("CountersController", CountersController);
}

/*angular.module("gdCameApp").controller('CountersController',
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

                    updateCounters(value.counters, $rootScope);
                });

            function updateCounters(counters, scope) {
                counters.forEach(function(item) {
                    var oldCounter = scope.gameData.cash.counters.filter(function(counter) {
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
            }
        }
    ]);*/