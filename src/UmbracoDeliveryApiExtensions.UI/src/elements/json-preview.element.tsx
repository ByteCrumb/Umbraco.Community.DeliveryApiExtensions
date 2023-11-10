import JsonView from '@uiw/react-json-view';
import {type JsonViewProps} from '@uiw/react-json-view';
import {type UUIIconElement} from '@umbraco-ui/uui';

import defineReactElement from '../helpers/define-react-element';

const WebReactJsonComponent = (props: Pick<JsonViewProps<Record<string, unknown>>, 'value'>) =>
  <JsonView displayDataTypes={false} shortenTextAfterLength={50} collapsed={2} value={props.value}>
    <JsonView.Null render={(props, {type}) => type === 'value' ? <span {...props}>null</span> : <span/>}/>
    <JsonView.CountInfo render={(_props, {value}) => Array.isArray(value) ? undefined : <span/> }/>
    <JsonView.Ellipsis render={(_props, {value}) => Object.keys(value ?? {}).length === 0 ? <span>&nbsp;</span> : undefined }/>
    <JsonView.Copied render={(props, _result) => {
      const copied = 'data-copied' in props && Boolean(props['data-copied']);
      return <uui-icon name='copy' style={{...props.style, color: copied ? 'var(--uui-color-positive)' : 'var(--uui-color-disabled-contrast)'}} onClick={copied ? undefined : props.onClick as unknown as React.MouseEventHandler<UUIIconElement>}></uui-icon>;
    }}/>
  </JsonView>;

defineReactElement('bc-json-preview', WebReactJsonComponent, {props: {value: undefined}});
