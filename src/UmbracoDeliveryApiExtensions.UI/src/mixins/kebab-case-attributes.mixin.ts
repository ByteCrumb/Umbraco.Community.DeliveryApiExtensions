import {type LitElement} from 'lit';
import type {PropertyDeclaration} from 'lit-element';

const camelCaseToKebabCase = (str: string) => str.replace(/([a-z0-9])([A-Z])/g, '$1-$2').toLowerCase();

export const KebabCaseAttributesMixin = <T extends typeof LitElement>(superClass: T) => {
  class KebabCaseAttributesMixinClass extends (superClass as typeof LitElement) {
    static createProperty(name: PropertyKey, options?: PropertyDeclaration) {
      let customOptions = options;

      // Derive the attribute name if not already defined or disabled
      if (typeof options?.attribute === 'undefined' || options?.attribute === true) {
        customOptions = {...options, attribute: camelCaseToKebabCase(name.toString())};
      }

      super.createProperty(name, customOptions);
    }
  }

  return KebabCaseAttributesMixinClass as T;
};
