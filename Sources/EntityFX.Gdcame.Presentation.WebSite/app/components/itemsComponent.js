angular
    .module("gdCameApp")
    .component("items",
    {
        templateUrl: function (viewTheme) {
            return "/app/views/" + viewTheme + "/itemsView.html";
        },
        controller: 'ItemsController'
    });