import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'OnlineServer',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44325/',
    redirectUri: baseUrl,
    clientId: 'OnlineServer_App',
    responseType: 'code',
    scope: 'offline_access OnlineServer',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://localhost:44325',
      rootNamespace: 'Qna.Game.OnlineServer',
    },
  },
} as Environment;
