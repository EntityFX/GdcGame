angular
    .module("gdCameApp")
    .component("counters",
    {
        templateUrl: function (viewTheme) {
            return "/app/views/" + viewTheme + "/countersView.html";
        },
        controller: 'CountersController'
    });