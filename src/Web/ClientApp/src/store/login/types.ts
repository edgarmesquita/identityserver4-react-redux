export interface IExternalProvider {
    authenticationScheme: string;
    displayName: string;
}
export interface ILoginResult {
    allowRememberLogin: boolean;
    enableLocalLogin: boolean;
    externalProviders: IExternalProvider[];
    visibleExternalProviders: IExternalProvider[];
    isExternalLoginOnly: boolean;
    externalLoginScheme: string;
    returnUrl?: string;
}

export interface ILoginState
{
    info?: ILoginResult;
    loading: boolean;
}