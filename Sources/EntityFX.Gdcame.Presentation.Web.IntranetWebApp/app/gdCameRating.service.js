angular
    .module("gdCameApp")
    .factory("gdCameRatingApiService",
        function ($rootScope, $http, $location) {
            var gameApiServiceBaseUri = $location.protocol() + "://" + $location.host() + ":" + $location.port() + "/api/RatingApi/";

            var gdCameRatingApiServiceFactory = {
                getUsersRatingByCount: function (count) {
                    return $http.post(gameApiServiceBaseUri + "GetUsersRatingByCount/", count)
                        .then(function (result) {
                            return result;
                        });
                },

                findUserRatingByUserName: function () {
                    return $http.get(gameApiServiceBaseUri + "FindUserRatingByUserName/")
                        .then(function (result) {
                            return result;
                        });
                },

                findUserRatingByUserNameAndAroundUsers: function (count) {
                    return $http.post(gameApiServiceBaseUri + "FindUserRatingByUserNameAndAroundUsers/", count)
                        .then(function (result) {
                            return result;
                        });
                }

            }

            return gdCameRatingApiServiceFactory;
        });