/**
 * Generated by orval v6.19.1 🍺
 * Do not edit manually.
 * Umbraco Delivery API
 * You can find out more about the Umbraco Delivery API in [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api).
 * OpenAPI spec version: Latest
 */
import axios from 'axios'
import type {
  AxiosRequestConfig,
  AxiosResponse
} from 'axios'
export type GetMediaItemById404 = ProblemDetails | HttpValidationProblemDetails;

export type GetMediaItemByIdParams = {
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetMediaItemByPath404 = ProblemDetails | HttpValidationProblemDetails;

export type GetMediaItemByPathParams = {
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetMediaItemParams = {
id?: string[];
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetMedia400 = ProblemDetails | HttpValidationProblemDetails;

export type GetMediaParams = {
/**
 * Specifies the media items to fetch. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
fetch?: string;
/**
 * Defines how to filter the fetched media items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
filter?: string[];
/**
 * Defines how to sort the found media items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
sort?: string[];
/**
 * Specifies the number of found media items to skip. Use this to control pagination of the response.
 */
skip?: number;
/**
 * Specifies the number of found media items to take. Use this to control pagination of the response.
 */
take?: number;
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetContentItemById404 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemById403 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemById401 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemByIdParams = {
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetContentItemByPath404 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemByPath403 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemByPath401 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemByPathParams = {
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetContentItem403 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItem401 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentItemParams = {
id?: string[];
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type GetContent404 = ProblemDetails | HttpValidationProblemDetails;

export type GetContent400 = ProblemDetails | HttpValidationProblemDetails;

export type GetContentParams = {
/**
 * Specifies the content items to fetch. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
fetch?: string;
/**
 * Defines how to filter the fetched content items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
filter?: string[];
/**
 * Defines how to sort the found content items. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
sort?: string[];
/**
 * Specifies the number of found content items to skip. Use this to control pagination of the response.
 */
skip?: number;
/**
 * Specifies the number of found content items to take. Use this to control pagination of the response.
 */
take?: number;
/**
 * Defines the properties that should be expanded in the response. Refer to [the documentation](https://docs.umbraco.com/umbraco-cms/reference/content-delivery-api#query-parameters) for more details on this.
 */
expand?: string;
};

export type TestPagePropertiesModelUserPicker = unknown | null;

export type TestPagePropertiesModelMediaPickerItem = ApiMediaWithCropsModel | ApiMediaWithCropsResponseModel;

export type TestPagePropertiesModel = TestCompositionPropertiesModel & TestComposition2PropertiesModel & {
  blockGrid?: ApiBlockGridModel;
  blockList?: ApiBlockListModel;
  checkboxList?: string[] | null;
  colorPicker?: PickedColorModel;
  contentPicker?: IApiContentModel;
  datePicker?: string | null;
  datePickerWithTime?: string | null;
  decimal?: number | null;
  dropdown?: string | null;
  email?: string | null;
  eyeDropperColorPicker?: string | null;
  imageCropper?: ApiImageCropperValueModel;
  markdown?: string | null;
  mediaPicker?: TestPagePropertiesModelMediaPickerItem[] | null;
  memberGroupPicker?: string[] | null;
  memberPicker?: string | null;
  multinodeTreepicker?: IApiContentModel[] | null;
  numeric?: number | null;
  radiobox?: string | null;
  repeatableTextstrings?: string[] | null;
  richText?: RichTextModel;
  slider?: number | null;
  tags?: string[] | null;
  textArea?: string | null;
  textString?: string | null;
  toggle?: boolean | null;
  uploadFile?: string | null;
  urlPicker?: ApiLinkModel[] | null;
  userPicker?: TestPagePropertiesModelUserPicker;
};

export type TestPageInvariantPropertiesModelUserPicker = unknown | null;

export type TestPageInvariantPropertiesModelMediaPickerItem = ApiMediaWithCropsModel | ApiMediaWithCropsResponseModel;

export type TestPageInvariantPropertiesModel = TestCompositionPropertiesModel & TestComposition2PropertiesModel & {
  blockGrid?: ApiBlockGridModel;
  blockList?: ApiBlockListModel;
  checkboxList?: string[] | null;
  colorPicker?: PickedColorModel;
  contentPicker?: IApiContentModel;
  datePicker?: string | null;
  datePickerWithTime?: string | null;
  decimal?: number | null;
  dropdown?: string | null;
  email?: string | null;
  eyeDropperColorPicker?: string | null;
  imageCropper?: ApiImageCropperValueModel;
  markdown?: string | null;
  mediaPicker?: TestPageInvariantPropertiesModelMediaPickerItem[] | null;
  memberGroupPicker?: string[] | null;
  memberPicker?: string | null;
  multinodeTreepicker?: IApiContentModel[] | null;
  numeric?: number | null;
  radiobox?: string | null;
  repeatableTextstrings?: string[] | null;
  richText?: RichTextModel;
  slider?: number | null;
  tags?: string[] | null;
  textArea?: string | null;
  textString?: string | null;
  toggle?: boolean | null;
  uploadFile?: string | null;
  urlPicker?: ApiLinkModel[] | null;
  userPicker?: TestPageInvariantPropertiesModelUserPicker;
};

export type TestPageInvariantContentResponseModelContentType = typeof TestPageInvariantContentResponseModelContentType[keyof typeof TestPageInvariantContentResponseModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestPageInvariantContentResponseModelContentType = {
  testPageInvariant: 'testPageInvariant',
} as const;

export type TestPageInvariantContentResponseModel = IApiContentResponseModelBase & TestPageInvariantContentModel & {
  contentType: TestPageInvariantContentResponseModelContentType;
};

export type TestPageInvariantContentModelContentType = typeof TestPageInvariantContentModelContentType[keyof typeof TestPageInvariantContentModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestPageInvariantContentModelContentType = {
  testPageInvariant: 'testPageInvariant',
} as const;

export type TestPageInvariantContentModel = IApiContentModelBase & {
  contentType: TestPageInvariantContentModelContentType;
  properties?: TestPageInvariantPropertiesModel;
};

export type TestPageContentResponseModelContentType = typeof TestPageContentResponseModelContentType[keyof typeof TestPageContentResponseModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestPageContentResponseModelContentType = {
  testPage: 'testPage',
} as const;

export type TestPageContentModelContentType = typeof TestPageContentModelContentType[keyof typeof TestPageContentModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestPageContentModelContentType = {
  testPage: 'testPage',
} as const;

export type TestPageContentModel = IApiContentModelBase & {
  contentType: TestPageContentModelContentType;
  properties?: TestPagePropertiesModel;
};

export type TestPageContentResponseModel = IApiContentResponseModelBase & TestPageContentModel & {
  contentType: TestPageContentResponseModelContentType;
};

export interface TestCompositionPropertiesModel {
  sharedString?: string | null;
  sharedToggle?: boolean | null;
}

export type TestCompositionElementModelContentType = typeof TestCompositionElementModelContentType[keyof typeof TestCompositionElementModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestCompositionElementModelContentType = {
  testComposition: 'testComposition',
} as const;

export type TestCompositionElementModel = IApiElementModelBase & {
  contentType: TestCompositionElementModelContentType;
  properties?: TestCompositionPropertiesModel;
};

export interface TestComposition2PropertiesModel {
  sharedRadiobox?: string | null;
  sharedRichText?: RichTextModel;
}

export type TestComposition2ElementModelContentType = typeof TestComposition2ElementModelContentType[keyof typeof TestComposition2ElementModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestComposition2ElementModelContentType = {
  testComposition2: 'testComposition2',
} as const;

export type TestComposition2ElementModel = IApiElementModelBase & {
  contentType: TestComposition2ElementModelContentType;
  properties?: TestComposition2PropertiesModel;
};

export type TestBlockPropertiesModel = TestCompositionPropertiesModel & {
  blocks?: ApiBlockListModel;
  multinodeTreepicker?: IApiContentModel[] | null;
  string?: string | null;
};

export type TestBlockElementModelContentType = typeof TestBlockElementModelContentType[keyof typeof TestBlockElementModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestBlockElementModelContentType = {
  testBlock: 'testBlock',
} as const;

export type TestBlockElementModel = IApiElementModelBase & {
  contentType: TestBlockElementModelContentType;
  properties?: TestBlockPropertiesModel;
};

export type TestBlock2PropertiesModel = TestCompositionPropertiesModel & {
  thisIsTestBlock2?: string | null;
};

export type TestBlock2ElementModelContentType = typeof TestBlock2ElementModelContentType[keyof typeof TestBlock2ElementModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const TestBlock2ElementModelContentType = {
  testBlock2: 'testBlock2',
} as const;

export type TestBlock2ElementModel = IApiElementModelBase & {
  contentType: TestBlock2ElementModelContentType;
  properties?: TestBlock2PropertiesModel;
};

export interface RichTextModel {
  markup?: string;
}

export interface ProblemDetails {
  detail?: string | null;
  instance?: string | null;
  status?: number | null;
  title?: string | null;
  type?: string | null;
  [key: string]: unknown;
}

export interface PickedColorModel {
  color?: string;
  label?: string;
}

export interface PagedIApiContentResponseModel {
  items: IApiContentResponseModel[];
  total: number;
}

export interface PagedApiMediaWithCropsResponseModel {
  items: ApiMediaWithCropsResponseModel[];
  total: number;
}

export type LinkTypeModel = typeof LinkTypeModel[keyof typeof LinkTypeModel];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const LinkTypeModel = {
  Content: 'Content',
  Media: 'Media',
  External: 'External',
} as const;

export interface ImageCropperFocalPointModel {
  left?: number;
  top?: number;
}

export interface ImageCropperCropCoordinatesModel {
  x1?: number;
  x2?: number;
  y1?: number;
  y2?: number;
}

export interface ImageCropperCropModel {
  alias?: string | null;
  coordinates?: ImageCropperCropCoordinatesModel;
  height?: number;
  width?: number;
}

export type IApiElementModelBaseProperties = {[key: string]: unknown};

export interface IApiElementModelBase {
  readonly contentType?: string;
  readonly id?: string;
  readonly properties?: IApiElementModelBaseProperties;
}

export type IApiElementModel = BlockSettingsElementModel | TestCompositionElementModel | TestComposition2ElementModel | TestBlockElementModel | TestBlock2ElementModel;

export interface IApiContentStartItemModel {
  readonly id?: string;
  readonly path?: string;
}

export interface IApiContentRouteModel {
  readonly path?: string;
  startItem?: IApiContentStartItemModel;
}

export type IApiContentResponseModelBaseProperties = {[key: string]: unknown};

export type IApiContentResponseModelBaseCultures = {[key: string]: IApiContentRouteModel};

export type IApiContentResponseModelBase = IApiContentModelBase & {
  readonly contentType?: string;
  readonly createDate?: string;
  readonly cultures?: IApiContentResponseModelBaseCultures;
  readonly id?: string;
  readonly name?: string | null;
  readonly properties?: IApiContentResponseModelBaseProperties;
  route?: IApiContentRouteModel;
  readonly updateDate?: string;
};

export type IApiContentResponseModel = TestPageContentResponseModel | TestPageInvariantContentResponseModel;

export type IApiContentModelBaseProperties = {[key: string]: unknown};

export type IApiContentModelBase = IApiElementModelBase & {
  readonly contentType?: string;
  readonly createDate?: string;
  readonly id?: string;
  readonly name?: string | null;
  readonly properties?: IApiContentModelBaseProperties;
  route?: IApiContentRouteModel;
  readonly updateDate?: string;
};

export type IApiContentModel = TestPageContentModel | TestPageInvariantContentModel;

export type HttpValidationProblemDetailsErrors = {[key: string]: string[]};

export type HttpValidationProblemDetails = ProblemDetails & {
  readonly errors?: HttpValidationProblemDetailsErrors;
  [key: string]: unknown;
};

export interface BlockSettingsPropertiesModel {
  anchorId?: string | null;
}

export type BlockSettingsElementModelContentType = typeof BlockSettingsElementModelContentType[keyof typeof BlockSettingsElementModelContentType];


// eslint-disable-next-line @typescript-eslint/no-redeclare
export const BlockSettingsElementModelContentType = {
  blockSettings: 'blockSettings',
} as const;

export type BlockSettingsElementModel = IApiElementModelBase & {
  contentType: BlockSettingsElementModelContentType;
  properties?: BlockSettingsPropertiesModel;
};

export type ApiMediaWithCropsModelProperties = {[key: string]: unknown};

export interface ApiMediaWithCropsModel {
  readonly bytes?: number | null;
  crops?: ImageCropperCropModel[] | null;
  readonly extension?: string | null;
  focalPoint?: ImageCropperFocalPointModel;
  readonly height?: number | null;
  readonly id?: string;
  readonly mediaType?: string;
  readonly name?: string;
  readonly properties?: ApiMediaWithCropsModelProperties;
  readonly url?: string;
  readonly width?: number | null;
}

export type ApiMediaWithCropsResponseModel = ApiMediaWithCropsModel & {
  createDate?: string;
  path?: string;
  updateDate?: string;
};

export interface ApiLinkModel {
  readonly destinationId?: string | null;
  readonly destinationType?: string | null;
  linkType?: LinkTypeModel;
  route?: IApiContentRouteModel;
  readonly target?: string | null;
  readonly title?: string | null;
  readonly url?: string | null;
}

export interface ApiImageCropperValueModel {
  crops?: ImageCropperCropModel[] | null;
  focalPoint?: ImageCropperFocalPointModel;
  url?: string;
}

export interface ApiBlockListModel {
  items?: ApiBlockListModelItemsItem[];
}

export interface ApiBlockItemModel {
  content?: IApiElementModel;
  settings?: IApiElementModel;
}

export interface ApiBlockGridAreaModel {
  alias?: string;
  columnSpan?: number;
  items?: ApiBlockGridItemModel[];
  rowSpan?: number;
}

export type ApiBlockGridItemModel = ApiBlockItemModel & {
  areaGridColumns?: number;
  areas?: ApiBlockGridAreaModel[];
  columnSpan?: number;
  rowSpan?: number;
};

export type ApiBlockListModelItemsItem = ApiBlockItemModel | ApiBlockGridItemModel;

export interface ApiBlockGridModel {
  gridColumns?: number;
  items?: ApiBlockGridItemModel[];
}





  export const getContent = <TData = AxiosResponse<PagedIApiContentResponseModel>>(
    params?: GetContentParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/content`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getContentItem = <TData = AxiosResponse<IApiContentResponseModel[]>>(
    params?: GetContentItemParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/content/item`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getContentItemByPath = <TData = AxiosResponse<IApiContentResponseModel>>(
    path: string,
    params?: GetContentItemByPathParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/content/item/${path}`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getContentItemById = <TData = AxiosResponse<IApiContentResponseModel>>(
    id: string,
    params?: GetContentItemByIdParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/content/item/${id}`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getMedia = <TData = AxiosResponse<PagedApiMediaWithCropsResponseModel>>(
    params?: GetMediaParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/media`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getMediaItem = <TData = AxiosResponse<ApiMediaWithCropsResponseModel[]>>(
    params?: GetMediaItemParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/media/item`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getMediaItemByPath = <TData = AxiosResponse<ApiMediaWithCropsResponseModel>>(
    path: string,
    params?: GetMediaItemByPathParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/media/item/${path}`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export const getMediaItemById = <TData = AxiosResponse<ApiMediaWithCropsResponseModel>>(
    id: string,
    params?: GetMediaItemByIdParams, options?: AxiosRequestConfig
 ): Promise<TData> => {
    return axios.get(
      `http://localhost:34962/umbraco/delivery/api/v1/media/item/${id}`,{
    ...options,
        params: {...params, ...options?.params},}
    );
  }

export type GetContentResult = AxiosResponse<PagedIApiContentResponseModel>
export type GetContentItemResult = AxiosResponse<IApiContentResponseModel[]>
export type GetContentItemByPathResult = AxiosResponse<IApiContentResponseModel>
export type GetContentItemByIdResult = AxiosResponse<IApiContentResponseModel>
export type GetMediaResult = AxiosResponse<PagedApiMediaWithCropsResponseModel>
export type GetMediaItemResult = AxiosResponse<ApiMediaWithCropsResponseModel[]>
export type GetMediaItemByPathResult = AxiosResponse<ApiMediaWithCropsResponseModel>
export type GetMediaItemByIdResult = AxiosResponse<ApiMediaWithCropsResponseModel>
