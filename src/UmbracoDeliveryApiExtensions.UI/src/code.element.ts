import {LitElement, type PropertyValueMap, css, html} from 'lit';
import {unsafeHTML} from 'lit/directives/unsafe-html.js';
import {customElement, property, state} from 'lit/decorators.js';
import Prism from 'prismjs';
import 'prismjs/components/prism-json';

Prism.manual = true;

/**
 * The Delivery Api Extensions Code element.
 */
@customElement('dae-code')
export class CodeElement extends LitElement {
  static styles = css`
  code[class*="language-"],
  pre[class*="language-"] {
    color: #c5c8c6;
    text-shadow: 0 1px rgba(0, 0, 0, 0.3);
    font-family: Inconsolata, Monaco, Consolas, 'Courier New', Courier, monospace;
    direction: ltr;
    text-align: left;
    white-space: pre;
    word-spacing: normal;
    word-break: normal;
    line-height: 1.5;

    -moz-tab-size: 4;
    -o-tab-size: 4;
    tab-size: 4;

    -webkit-hyphens: none;
    -moz-hyphens: none;
    -ms-hyphens: none;
    hyphens: none;
  }

  /* Code blocks */
  pre[class*="language-"] {
    padding: 1em;
    margin: .5em 0;
    overflow: auto;
    border-radius: 0.3em;
  }

  :not(pre) > code[class*="language-"],
  pre[class*="language-"] {
    background: #1d1f21;
  }

  /* Inline code */
  :not(pre) > code[class*="language-"] {
    padding: .1em;
    border-radius: .3em;
  }

  .token.comment,
  .token.prolog,
  .token.doctype,
  .token.cdata {
    color: #7C7C7C;
  }

  .token.punctuation {
    color: #c5c8c6;
  }

  .namespace {
    opacity: .7;
  }

  .token.property,
  .token.keyword,
  .token.tag {
    color: #96CBFE;
  }

  .token.class-name {
    color: #FFFFB6;
    text-decoration: underline;
  }

  .token.boolean,
  .token.constant {
    color: #99CC99;
  }

  .token.symbol,
  .token.deleted {
    color: #f92672;
  }

  .token.number {
    color: #FF73FD;
  }

  .token.selector,
  .token.attr-name,
  .token.string,
  .token.char,
  .token.builtin,
  .token.inserted {
    color: #A8FF60;
  }

  .token.variable {
    color: #C6C5FE;
  }

  .token.operator {
    color: #EDEDED;
  }

  .token.entity {
    color: #FFFFB6;
    cursor: help;
  }

  .token.url {
    color: #96CBFE;
  }

  .language-css .token.string,
  .style .token.string {
    color: #87C38A;
  }

  .token.atrule,
  .token.attr-value {
    color: #F9EE98;
  }

  .token.function {
    color: #DAD085;
  }

  .token.regex {
    color: #E9C062;
  }

  .token.important {
    color: #fd971f;
  }

  .token.important,
  .token.bold {
    font-weight: bold;
  }

  .token.italic {
    font-style: italic;
  }
  `;

  @property({type: String})
    code = '';

  @property({type: String})
    language = '';

  @state()
  private _highlightedCode = '';

  render() {
    return html`
      <pre class="language-${this.language}">${unsafeHTML(this._highlightedCode)}</pre>
      `;
  }

  protected updated(_changedProperties: PropertyValueMap<any> | Map<PropertyKey, unknown>): void {
    this._highlightedCode = Prism.highlight(this.code, Prism.languages[this.language], this.language);
  }
}

declare global {
  interface HTMLElementTagNameMap {
    'dae-code': CodeElement;
  }
}
