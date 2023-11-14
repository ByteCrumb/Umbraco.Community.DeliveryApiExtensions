/**
 * This file was auto-generated by openapi-typescript.
 * Do not make direct changes to the file.
 */


export interface paths {
  "/umbraco/delivery/api/v1/content": {
    get: operations["GetContent"];
  };
  "/umbraco/delivery/api/v1/content/item": {
    get: operations["GetContentItem"];
  };
  "/umbraco/delivery/api/v1/content/item/{path}": {
    get: operations["GetContentItemByPath"];
  };
  "/umbraco/delivery/api/v1/content/item/{id}": {
    get: operations["GetContentItemById"];
  };
  "/umbraco/delivery/api/v1/media": {
    get: operations["GetMedia"];
  };
  "/umbraco/delivery/api/v1/media/item": {
    get: operations["GetMediaItem"];
  };
  "/umbraco/delivery/api/v1/media/item/{path}": {
    get: operations["GetMediaItemByPath"];
  };
  "/umbraco/delivery/api/v1/media/item/{id}": {
    get: operations["GetMediaItemById"];
  };
}

export type webhooks = Record<string, never>;

export interface components {
  schemas: {
    ApiBlockGridAreaModel: {
      alias?: string;
      /** Format: int32 */
      rowSpan?: number;
      /** Format: int32 */
      columnSpan?: number;
      items?: components["schemas"]["ApiBlockGridItemModel"][];
    };
    ApiBlockGridItemModel: {
      /** Format: int32 */
      rowSpan?: number;
      /** Format: int32 */
      columnSpan?: number;
      /** Format: int32 */
      areaGridColumns?: number;
      areas?: components["schemas"]["ApiBlockGridAreaModel"][];
    } & components["schemas"]["ApiBlockItemModel"];
    ApiBlockGridModel: {
      /** Format: int32 */
      gridColumns?: number;
      items?: components["schemas"]["ApiBlockGridItemModel"][];
    };
    ApiBlockItemModel: {
      content?: components["schemas"]["IApiElementModel"];
      settings?: components["schemas"]["IApiElementModel"];
    };
    ApiBlockListModel: {
      items?: (components["schemas"]["ApiBlockItemModel"] | components["schemas"]["ApiBlockGridItemModel"])[];
    };
    ApiImageCropperValueModel: {
      url?: string;
      focalPoint?: components["schemas"]["ImageCropperFocalPointModel"];
      crops?: components["schemas"]["ImageCropperCropModel"][] | null;
    };
    ApiLinkModel: {
      url?: string | null;
      title?: string | null;
      target?: string | null;
      /** Format: uuid */
      destinationId?: string | null;
      destinationType?: string | null;
      route?: components["schemas"]["IApiContentRouteModel"];
      linkType?: components["schemas"]["LinkTypeModel"];
    };
    ApiMediaWithCropsModel: {
      /** Format: uuid */
      id?: string;
      name?: string;
      mediaType?: string;
      url?: string;
      extension?: string | null;
      /** Format: int32 */
      width?: number | null;
      /** Format: int32 */
      height?: number | null;
      /** Format: int32 */
      bytes?: number | null;
      properties?: {
        [key: string]: unknown;
      };
      focalPoint?: components["schemas"]["ImageCropperFocalPointModel"];
      crops?: components["schemas"]["ImageCropperCropModel"][] | null;
    };
    ApiMediaWithCropsResponseModel: {
      path?: string;
      /** Format: date-time */
      createDate?: string;
      /** Format: date-time */
      updateDate?: string;
    } & components["schemas"]["ApiMediaWithCropsModel"];
    BlockSettingsElementModel: {
      contentType: "blockSettings";
      properties?: components["schemas"]["BlockSettingsPropertiesModel"];
    } & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    BlockSettingsPropertiesModel: {
      anchorId?: string | null;
    };
    HttpValidationProblemDetails: {
      errors?: {
        [key: string]: string[];
      };
      [key: string]: unknown;
    } & components["schemas"]["ProblemDetails"];
    IApiContentModel: components["schemas"]["TestPageContentModel"] | components["schemas"]["TestPageInvariantContentModel"];
    IApiContentModelBase: ({
      contentType: "IApiContentModelBase";
      name?: string | null;
      /** Format: date-time */
      createDate?: string;
      /** Format: date-time */
      updateDate?: string;
      route?: components["schemas"]["IApiContentRouteModel"];
      /** Format: uuid */
      id?: string;
      contentType?: string;
      properties?: {
        [key: string]: unknown;
      };
    }) & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    IApiContentResponseModel: components["schemas"]["TestPageContentResponseModel"] | components["schemas"]["TestPageInvariantContentResponseModel"];
    IApiContentResponseModelBase: ({
      contentType: "IApiContentResponseModelBase";
      cultures?: {
        [key: string]: components["schemas"]["IApiContentRouteModel"];
      };
      name?: string | null;
      /** Format: date-time */
      createDate?: string;
      /** Format: date-time */
      updateDate?: string;
      route?: components["schemas"]["IApiContentRouteModel"];
      /** Format: uuid */
      id?: string;
      contentType?: string;
      properties?: {
        [key: string]: unknown;
      };
    }) & Omit<components["schemas"]["IApiContentModelBase"], "contentType">;
    IApiContentRouteModel: {
      path?: string;
      startItem?: components["schemas"]["IApiContentStartItemModel"];
    };
    IApiContentStartItemModel: {
      /** Format: uuid */
      id?: string;
      path?: string;
    };
    IApiElementModel: components["schemas"]["BlockSettingsElementModel"] | components["schemas"]["TestCompositionElementModel"] | components["schemas"]["TestComposition2ElementModel"] | components["schemas"]["TestBlockElementModel"] | components["schemas"]["TestBlock2ElementModel"];
    IApiElementModelBase: {
      /** Format: uuid */
      id?: string;
      contentType?: string;
      properties?: {
        [key: string]: unknown;
      };
    };
    ImageCropperCropCoordinatesModel: {
      /** Format: double */
      x1?: number;
      /** Format: double */
      y1?: number;
      /** Format: double */
      x2?: number;
      /** Format: double */
      y2?: number;
    };
    ImageCropperCropModel: {
      alias?: string | null;
      /** Format: int32 */
      width?: number;
      /** Format: int32 */
      height?: number;
      coordinates?: components["schemas"]["ImageCropperCropCoordinatesModel"];
    };
    ImageCropperFocalPointModel: {
      /** Format: double */
      left?: number;
      /** Format: double */
      top?: number;
    };
    /** @enum {string} */
    LinkTypeModel: "Content" | "Media" | "External";
    PagedApiMediaWithCropsResponseModel: {
      /** Format: int64 */
      total: number;
      items: components["schemas"]["ApiMediaWithCropsResponseModel"][];
    };
    PagedIApiContentResponseModel: {
      /** Format: int64 */
      total: number;
      items: components["schemas"]["IApiContentResponseModel"][];
    };
    PickedColorModel: {
      color?: string;
      label?: string;
    };
    ProblemDetails: {
      type?: string | null;
      title?: string | null;
      /** Format: int32 */
      status?: number | null;
      detail?: string | null;
      instance?: string | null;
      [key: string]: unknown;
    };
    RichTextModel: {
      markup?: string;
    };
    TestBlock2ElementModel: {
      contentType: "testBlock2";
      properties?: components["schemas"]["TestBlock2PropertiesModel"];
    } & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    TestBlock2PropertiesModel: ({
      thisIsTestBlock2?: string | null;
    }) & components["schemas"]["TestCompositionPropertiesModel"];
    TestBlockElementModel: {
      contentType: "testBlock";
      properties?: components["schemas"]["TestBlockPropertiesModel"];
    } & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    TestBlockPropertiesModel: ({
      string?: string | null;
      multinodeTreepicker?: components["schemas"]["IApiContentModel"][] | null;
      blocks?: components["schemas"]["ApiBlockListModel"];
    }) & components["schemas"]["TestCompositionPropertiesModel"];
    TestComposition2ElementModel: {
      contentType: "testComposition2";
      properties?: components["schemas"]["TestComposition2PropertiesModel"];
    } & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    TestComposition2PropertiesModel: {
      sharedRadiobox?: string | null;
      sharedRichText?: components["schemas"]["RichTextModel"];
    };
    TestCompositionElementModel: {
      contentType: "testComposition";
      properties?: components["schemas"]["TestCompositionPropertiesModel"];
    } & Omit<components["schemas"]["IApiElementModelBase"], "contentType">;
    TestCompositionPropertiesModel: {
      sharedToggle?: boolean | null;
      sharedString?: string | null;
    };
    TestPageContentModel: {
      contentType: "testPage";
      properties?: components["schemas"]["TestPagePropertiesModel"];
    } & Omit<components["schemas"]["IApiContentModelBase"], "contentType">;
    TestPageContentResponseModel: {
      contentType: "testPage";
    } & Omit<components["schemas"]["IApiContentResponseModelBase"], "contentType"> & components["schemas"]["TestPageContentModel"];
    TestPageInvariantContentModel: {
      contentType: "testPageInvariant";
      properties?: components["schemas"]["TestPageInvariantPropertiesModel"];
    } & Omit<components["schemas"]["IApiContentModelBase"], "contentType">;
    TestPageInvariantContentResponseModel: {
      contentType: "testPageInvariant";
    } & Omit<components["schemas"]["IApiContentResponseModelBase"], "contentType"> & components["schemas"]["TestPageInvariantContentModel"];
    TestPageInvariantPropertiesModel: ({
      textString?: string | null;
      textArea?: string | null;
      /** Format: date-time */
      datePickerWithTime?: string | null;
      /** Format: date-time */
      datePicker?: string | null;
      toggle?: boolean | null;
      /** Format: int32 */
      numeric?: number | null;
      /** Format: double */
      decimal?: number | null;
      /** Format: double */
      slider?: number | null;
      tags?: string[] | null;
      email?: string | null;
      colorPicker?: components["schemas"]["PickedColorModel"];
      contentPicker?: components["schemas"]["IApiContentModel"];
      eyeDropperColorPicker?: string | null;
      urlPicker?: components["schemas"]["ApiLinkModel"][] | null;
      multinodeTreepicker?: components["schemas"]["IApiContentModel"][] | null;
      richText?: components["schemas"]["RichTextModel"];
      blockGrid?: components["schemas"]["ApiBlockGridModel"];
      markdown?: string | null;
      memberGroupPicker?: string[] | null;
      memberPicker?: string | null;
      userPicker?: Record<string, unknown> | null;
      blockList?: components["schemas"]["ApiBlockListModel"];
      checkboxList?: string[] | null;
      dropdown?: string | null;
      radiobox?: string | null;
      repeatableTextstrings?: string[] | null;
      uploadFile?: string | null;
      imageCropper?: components["schemas"]["ApiImageCropperValueModel"];
      mediaPicker?: ((components["schemas"]["ApiMediaWithCropsModel"] | components["schemas"]["ApiMediaWithCropsResponseModel"])[]) | null;
    }) & components["schemas"]["TestCompositionPropertiesModel"] & components["schemas"]["TestComposition2PropertiesModel"];
    TestPagePropertiesModel: ({
      textString?: string | null;
      textArea?: string | null;
      /** Format: date-time */
      datePickerWithTime?: string | null;
      /** Format: date-time */
      datePicker?: string | null;
      toggle?: boolean | null;
      /** Format: int32 */
      numeric?: number | null;
      /** Format: double */
      decimal?: number | null;
      /** Format: double */
      slider?: number | null;
      tags?: string[] | null;
      email?: string | null;
      colorPicker?: components["schemas"]["PickedColorModel"];
      contentPicker?: components["schemas"]["IApiContentModel"];
      eyeDropperColorPicker?: string | null;
      urlPicker?: components["schemas"]["ApiLinkModel"][] | null;
      multinodeTreepicker?: components["schemas"]["IApiContentModel"][] | null;
      richText?: components["schemas"]["RichTextModel"];
      blockGrid?: components["schemas"]["ApiBlockGridModel"];
      markdown?: string | null;
      memberGroupPicker?: string[] | null;
      memberPicker?: string | null;
      userPicker?: Record<string, unknown> | null;
      blockList?: components["schemas"]["ApiBlockListModel"];
      checkboxList?: string[] | null;
      dropdown?: string | null;
      radiobox?: string | null;
      repeatableTextstrings?: string[] | null;
      uploadFile?: string | null;
      imageCropper?: components["schemas"]["ApiImageCropperValueModel"];
      mediaPicker?: ((components["schemas"]["ApiMediaWithCropsModel"] | components["schemas"]["ApiMediaWithCropsResponseModel"])[]) | null;
    }) & components["schemas"]["TestCompositionPropertiesModel"] & components["schemas"]["TestComposition2PropertiesModel"];
  };
  responses: never;
  parameters: never;
  requestBodies: never;
  headers: never;
  pathItems: never;
}

export type $defs = Record<string, never>;

export type external = Record<string, never>;

export interface operations {

  GetContent: {
    parameters: {
      query?: {
        /** @description Specifies the content items to fetch. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        fetch?: string;
        /** @description Defines how to filter the fetched content items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        filter?: string[];
        /** @description Defines how to sort the found content items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        sort?: string[];
        /** @description Specifies the number of found content items to skip. Use this to control pagination of the response. */
        skip?: number;
        /** @description Specifies the number of found content items to take. Use this to control pagination of the response. */
        take?: number;
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description Defines the language to return. Use this when querying language variant content items. */
        "Accept-Language"?: string;
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
        /** @description Whether to request draft content. */
        Preview?: boolean;
        /** @description URL segment or GUID of a root content item. */
        "Start-Item"?: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["PagedIApiContentResponseModel"];
        };
      };
      /** @description Bad Request */
      400: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Not Found */
      404: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetContentItem: {
    parameters: {
      query?: {
        id?: string[];
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description Defines the language to return. Use this when querying language variant content items. */
        "Accept-Language"?: string;
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
        /** @description Whether to request draft content. */
        Preview?: boolean;
        /** @description URL segment or GUID of a root content item. */
        "Start-Item"?: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["IApiContentResponseModel"][];
        };
      };
      /** @description Unauthorized */
      401: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Forbidden */
      403: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetContentItemByPath: {
    parameters: {
      query?: {
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description Defines the language to return. Use this when querying language variant content items. */
        "Accept-Language"?: string;
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
        /** @description Whether to request draft content. */
        Preview?: boolean;
        /** @description URL segment or GUID of a root content item. */
        "Start-Item"?: string;
      };
      path: {
        path: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["IApiContentResponseModel"];
        };
      };
      /** @description Unauthorized */
      401: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Forbidden */
      403: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Not Found */
      404: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetContentItemById: {
    parameters: {
      query?: {
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description Defines the language to return. Use this when querying language variant content items. */
        "Accept-Language"?: string;
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
        /** @description Whether to request draft content. */
        Preview?: boolean;
        /** @description URL segment or GUID of a root content item. */
        "Start-Item"?: string;
      };
      path: {
        id: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["IApiContentResponseModel"];
        };
      };
      /** @description Unauthorized */
      401: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Forbidden */
      403: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
      /** @description Not Found */
      404: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetMedia: {
    parameters: {
      query?: {
        /** @description Specifies the media items to fetch. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        fetch?: string;
        /** @description Defines how to filter the fetched media items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        filter?: string[];
        /** @description Defines how to sort the found media items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        sort?: string[];
        /** @description Specifies the number of found media items to skip. Use this to control pagination of the response. */
        skip?: number;
        /** @description Specifies the number of found media items to take. Use this to control pagination of the response. */
        take?: number;
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["PagedApiMediaWithCropsResponseModel"];
        };
      };
      /** @description Bad Request */
      400: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetMediaItem: {
    parameters: {
      query?: {
        id?: string[];
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["ApiMediaWithCropsResponseModel"][];
        };
      };
    };
  };
  GetMediaItemByPath: {
    parameters: {
      query?: {
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
      };
      path: {
        path: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["ApiMediaWithCropsResponseModel"];
        };
      };
      /** @description Not Found */
      404: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
  GetMediaItemById: {
    parameters: {
      query?: {
        /** @description Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this. */
        expand?: string;
      };
      header?: {
        /** @description API key specified through configuration to authorize access to the API. */
        "Api-Key"?: string;
      };
      path: {
        id: string;
      };
    };
    responses: {
      /** @description Success */
      200: {
        content: {
          "application/json": components["schemas"]["ApiMediaWithCropsResponseModel"];
        };
      };
      /** @description Not Found */
      404: {
        content: {
          "application/json": components["schemas"]["ProblemDetails"] | components["schemas"]["HttpValidationProblemDetails"];
        };
      };
    };
  };
}
