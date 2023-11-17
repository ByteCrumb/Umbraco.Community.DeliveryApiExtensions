import angular from 'angular';

angular
  .module('umbraco')
  .controller(
    'Umbraco.Community.DeliveryApiExtensions.Preview',
    [
      '$scope',
      '$routeParams',
      'contentAppHelper',
      'eventsService',
      function ($scope, $routeParams, contentAppHelper, eventsService) {
        const vm = this;

        registerAsKnownContentApp();

        vm.context = {
          apiPath: $scope.model.viewModel.apiPath,
          hasPreview: $scope.model.viewModel.entityType === 'document',
          culture: $routeParams.cculture || $routeParams.mculture,
          isPublished: $scope.variantContent?.state !== 'Draft',
          updateDate: $scope.variantContent?.updateDate,
        };

        const contentSavedWatch = eventsService.on('content.saved', updatePreview);
        vm.$onDestroy = function () {
          contentSavedWatch();
        };

        function updatePreview() {
          vm.context = {
            ...vm.context,
            isPublished: $scope.variantContent?.state !== 'Draft',
            updateDate: $scope.variantContent?.updateDate,
          };
        }

        // Ensures the content save/publish are displayed
        function registerAsKnownContentApp() {
          const contentAppAlias = 'deliveryApiPreview';
          if (contentAppHelper.CONTENT_BASED_APPS.indexOf(contentAppAlias) === -1) {
            contentAppHelper.CONTENT_BASED_APPS.push(contentAppAlias);
          }
        }
      },
    ],
  );
