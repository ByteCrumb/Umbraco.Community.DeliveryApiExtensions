import {createContext} from '@lit/context';

export interface ApiPreviewContext {
  readonly apiPath: string;
  readonly culture?: string;
  readonly updateDate?: string;
}

export const apiPreviewContext = createContext<ApiPreviewContext>('api-preview');
