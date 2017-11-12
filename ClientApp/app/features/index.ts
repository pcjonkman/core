import { FrameworkConfiguration } from 'aurelia-framework';
import { PLATFORM } from 'aurelia-pal';

export function configure(config: FrameworkConfiguration): void { // tslint:disable-line:only-arrow-functions
  const components: string[] = [
    PLATFORM.moduleName('./validation-summary.html')
  ];

  config.globalResources(components);
}
