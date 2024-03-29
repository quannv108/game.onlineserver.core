import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:8080';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'OnlineServer',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://dev.quannv.click:44325/',
    redirectUri: baseUrl,
    clientId: 'OnlineServer_App',
    responseType: 'code',
    scope: 'offline_access OnlineServer',
    requireHttps: true
  },
  apis: {
    default: {
      url: 'https://dev.quannv.click:44325',
      rootNamespace: 'Qna.Game.OnlineServer',
    },
  },
} as Environment;
