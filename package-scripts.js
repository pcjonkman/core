/* globals ,module,require */
const {concurrent, crossEnv, rimraf, series} = require('nps-utils');

module.exports = {
  scripts: {
    default: 'nps webpack',
    dev: 'nps webpack.dev',
    prod: 'nps webpack.build',
    build: 'nps webpack.build',
    lint: {
      default: series(
        'nps lint.app',
        'nps lint.scss'
      ),
      app: 'tslint -p tsconfig.app.json',
      scss: 'stylelint **/*.scss'
    },
    webpack: {
      default: 'nps webpack.build',
      build: {
        before: rimraf('wwwroot/dist'),
        default: series(
          'nps webpack.build.before',
          crossEnv('NODE_ENV=production webpack --progress -p --env.production --env.extractCss')
        )
      },
      dev: {
        before: rimraf('wwwroot/dist'),
        default: series(
          'nps webpack.build.before',
          crossEnv('NODE_ENV=development webpack --progress')
        )
      }

    }
  }
};
