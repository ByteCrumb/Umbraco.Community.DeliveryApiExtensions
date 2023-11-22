declare namespace umbraco {
  declare namespace services {
    interface IEventService {
      on: angular.IRootScopeService['$on'];
    }

    interface IContentAppHelper {
      readonly CONTENT_BASED_APPS: string[];
    }
  }
}
