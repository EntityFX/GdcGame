angular
    .module("gdCameApp")
    .factory("gdCameAdminApiService",
        function ($rootScope, $http, $location, $localStorage) {
            var gdCameAdminApiServiceFactory = {
                getStatistics: function(serverAddress) {
                    return $http.get(serverAddress + "/api/admin/statistics/")
                        .then(function(result) {
                            return result;
                        });
                }


        };
            return gdCameAdminApiServiceFactory;
        });