angular
    .module("gdCameApp")
    .component('fundDrivers',
    {
        templateUrl: '/app/templates/fundDrivers.template.html',
        controller: function FundDriversController($rootScope, $scope, gameData, gdCameApiService) {
            gameData.then(function (data) {
                $scope.fundsDrivers = data.FundsDrivers;
            });

            $.connection.hub.start();
            $scope.buyFundDriver = function(fundDriverId) {
                gdCameApiService.buyFundDriver(fundDriverId)
                .then(function (value) {
                    var itemIndex;
                    var oldFundDriver = $scope.fundsDrivers.filter(function (item, index) {
                        itemIndex = index;
                        return item.Id === value.data.FundsDriverBuyInfo.Id;
                    })[0];

                    if (oldFundDriver != undefined) {
                        $scope.fundsDrivers[itemIndex] = value.data.FundsDriverBuyInfo;
                        //$scope.$apply();
                    }
                });
            }
        }
    });
