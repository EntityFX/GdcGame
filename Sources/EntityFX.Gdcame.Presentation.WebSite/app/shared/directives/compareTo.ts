namespace Gdcame.Directives {
    var compareTo = () => ({
        require: "ngModel",
        scope: {
            otherModelValue: "=compareTo"
        },
        link(scope, element, attributes, ngModel) {

            ngModel.$validators.compareTo = modelValue => (modelValue === scope.otherModelValue);

            scope.$watch("otherModelValue", () => {
                ngModel.$validate();
            });
        }
    });

    angular.module("gdCameApp").directive("compareTo", compareTo);

    var loading = () => ({
        scope: {
            loadingValue: "="
        },
        link(scope, element, attributes) {
            var loadingElement = angular.element(attributes.loadingText);
            scope.$watch("loadingValue",
                (value) => {
                    if (scope.loadingValue) {
                        element.prepend(attributes.loadingText);
                    } else {
                        if (element.children(loadingElement).length > 0) {
                            angular.element(element.children(loadingElement)).remove();
                        }
                    }
                });
        }
    });
    angular.module("gdCameApp").directive("loading", loading);
}