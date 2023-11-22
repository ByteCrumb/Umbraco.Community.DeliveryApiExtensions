import angular from 'angular';

import {type ApiPreviewContext} from '../contexts/api-preview.context';

export interface PreviewControllerContext extends ApiPreviewContext {
  readonly hasPreview: boolean;
  readonly isPublished: boolean;
}

type Controller = angular.IController & {context?: PreviewControllerContext};
type Scope = angular.IScope & {
  model?: {
    viewModel?: {
      apiPath: string;
      entityType: 'document' | 'media';
    };
  };
  variantContent?: {
    state?: 'NotCreated' | 'Draft' | 'Published' | 'PublishedPendingChanges';
    updateDate?: string;
  };
};

angular
  .module('umbraco')
  .controller(
    'Umbraco.Community.DeliveryApiExtensions.Preview',
    [
      '$scope',
      '$routeParams',
      'contentAppHelper',
      'eventsService',
      function (this: Controller, $scope: Scope, $routeParams: angular.route.IRouteParamsService, contentAppHelper: umbraco.services.IContentAppHelper, eventsService: umbraco.services.IEventService) {
        const vm = this;

        registerAsKnownContentApp();

        if ($scope.model?.viewModel === undefined) {
          return;
        }

        vm.context = {
          apiPath: $scope.model.viewModel.apiPath ?? '',
          hasPreview: $scope.model.viewModel.entityType === 'document',
          culture: $routeParams.cculture as string || $routeParams.mculture as string || undefined,
          isPublished: $scope.variantContent?.state !== 'Draft',
          updateDate: $scope.variantContent?.updateDate,
        };

        const contentSavedWatch = eventsService.on('content.saved', updatePreview);
        vm.$onDestroy = () => {
          contentSavedWatch();
        };

        function updatePreview() {
          if (vm.context) {
            vm.context = {
              ...vm.context,
              isPublished: $scope.variantContent?.state !== 'Draft',
              updateDate: $scope.variantContent?.updateDate,
            };
          }
        }

        // Ensures the content save/publish are displayed
        function registerAsKnownContentApp() {
          const contentAppAlias = 'deliveryApiPreview';
          if (!contentAppHelper.CONTENT_BASED_APPS.includes(contentAppAlias)) {
            contentAppHelper.CONTENT_BASED_APPS.push(contentAppAlias);
          }
        }
      },
    ],
  );
