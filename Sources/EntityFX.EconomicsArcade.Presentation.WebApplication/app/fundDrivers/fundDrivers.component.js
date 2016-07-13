angular
    .module("gdCameApp")
    .component('fundDrivers',
    {
        templateUrl: '/app/templates/fundDrivers.template.html',
        controller: function FundDriversController($rootScope, $scope, gameData, gdCameApiService) {
            $scope.fundsDrivers = [];

            gameData.then(function (data) {
                $scope.fundsDrivers = data.FundsDrivers;
            });

            $scope.$on('update.fundsDrivers',
                function (event, value) {
                    $scope.fundsDrivers = value.FundsDrivers;
                });

            $scope.buyFundDriver = function (fundDriverId) {
                gdCameApiService.buyFundDriver(fundDriverId)
                .then(function (value) {
                    if (value.data == undefined) return;
                    var itemIndex;
                    var oldFundDriver = $scope.fundsDrivers.filter(function (item, index) {
                        itemIndex = index;
                        return item.Id === value.data.FundsDriverBuyInfo.Id;
                    })[0];

                    if (oldFundDriver != undefined) {
                        oldFundDriver.BuyCount = value.data.FundsDriverBuyInfo.BuyCount;
                        oldFundDriver.Value = value.data.FundsDriverBuyInfo.Value;

                        if (value.data.ModifiedCountersInfo != undefined) {
                            $rootScope.$broadcast('counters.update', value.data.ModifiedCountersInfo);
                        }
                    }
                });
            }
        }
    });
