angular
    .module("gdCameApp")
    .component('rating',
    {
        templateUrl: '/app/templates/rating.template.html',
        controller: function RatingController($rootScope, $scope, gdCameRatingApiService) {

            gdCameRatingApiService.getUsersRatingByCount(500)
                .then(function (value) {
                    $scope.ratings = value.data;
                });
        }
    });