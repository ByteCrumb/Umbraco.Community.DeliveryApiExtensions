import JsonView from '@uiw/react-json-view';
import {type JsonViewProps} from '@uiw/react-json-view';
import {type UUIIconElement} from '@umbraco-ui/uui';

import { lightTheme } from '@uiw/react-json-view/light';
import { vscodeTheme } from '@uiw/react-json-view/vscode';

import defineReactElement from '../helpers/define-react-element';

export interface JsonPreviewProps<T extends object> extends JsonViewProps<T> {
  theme?: 'dark' | 'light';
}

const WebReactJsonComponent = (props: JsonPreviewProps<Record<string, unknown>>) =>
  <JsonView displayDataTypes={false} shortenTextAfterLength={50} collapsed={2} value={props.value} style={ {...(props.theme == 'dark' ? vscodeTheme : lightTheme), backgroundColor: 'transparent'} }>
    <JsonView.Null render={(props, {type}) => type === 'value' ? <span {...props}>null</span> : <span/>}/>
    <JsonView.CountInfo render={(_props, {value}) => Array.isArray(value) ? undefined : <span/> }/>
    <JsonView.Ellipsis render={(_props, {value}) => Object.keys(value ?? {}).length === 0 ? <span>&nbsp;</span> : undefined }/>
    <JsonView.Copied render={(props, _result) => {
      const copied = 'data-copied' in props && Boolean(props['data-copied']);
      // @ts-ignore
      return <uui-icon name='copy' style={{...props.style, color: copied ? 'var(--uui-color-positive)' : 'var(--uui-color-disabled-contrast)'}} onClick={copied ? undefined : props.onClick as unknown as React.MouseEventHandler<UUIIconElement>}></uui-icon>;
    }}/>
  </JsonView>;

defineReactElement('bc-json-preview', WebReactJsonComponent, {props: {value: undefined, theme: 'string' }});
