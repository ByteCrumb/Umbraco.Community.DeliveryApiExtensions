// eslint-disable-next-line no-undef
angular.module('umbraco')
  .controller('Umbraco.Community.DeliveryApiExtensions.Preview', function ($scope, $routeParams, contentAppHelper) {
    const vm = this;

    registerAsKnownContentApp();

    vm.apiPath = $scope.model.viewModel.apiPath;
    vm.culture = $routeParams.cculture || $routeParams.mculture;

    updateIsPublished();
    const unwatch = $scope.$watch('variantContent.state', updateIsPublished);

    vm.$onDestroy = function () {
      unwatch();
    };

    function updateIsPublished() {
      vm.isPublished = $scope.variantContent?.state !== 'Draft';
    }

    // Ensures the content save/publish are displayed
    function registerAsKnownContentApp() {
      const contentAppAlias = 'deliveryApiPreview';
      if (contentAppHelper.CONTENT_BASED_APPS.indexOf(contentAppAlias) === -1) {
        contentAppHelper.CONTENT_BASED_APPS.push(contentAppAlias);
      }
    }
  });
