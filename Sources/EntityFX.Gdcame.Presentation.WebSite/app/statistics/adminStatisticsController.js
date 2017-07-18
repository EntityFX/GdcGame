angular.module("gdCameApp").controller('AdminStatisticsController', ["$location", "$rootScope", "$scope", "$localStorage", "$interval", "gdCameAdminApiService",
    function ($location, $rootScope, $scope, $localStorage, $interval, gdCameAdminApiService) {
        var statisticsInterval = $interval(updateStatistics, 1000);
        $scope.serversStatistics = {};

        function updateStatistics() {
            gdCameAdminApiService.getStatistics("http://localhost:9001")
                .then(function (value) {
                    $scope.serversStatistics["localhost"] = value.data;
                });
        }

        $scope.$on("$destroy", function () {
            $interval.cancel(statisticsInterval);
        });
    }]);