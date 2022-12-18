module.exports = {
    webpack: function (config, env) {
        return config;
    },
    jest: function (config) {
        return config;
    },
    devServer: function(configFunction) {
        return function(proxy, allowedHost) {
            const config = configFunction(proxy, allowedHost);
            const csp = 'default-src \'self\'; style-src \'self\' \'unsafe-inline\' https://fonts.googleapis.com; font-src \'self\' https://fonts.gstatic.com; object-src \'none\'; frame-ancestors \'none\'; sandbox allow-forms allow-same-origin allow-scripts; base-uri \'self\';';
            config.headers = {
                'X-Content-Type-Options': 'nosniff',
                'X-Frame-Options': 'SAMEORIGIN',
                'Content-Security-Policy': csp,
                'X-Content-Security-Policy': csp,
                'Referrer-Policy': 'no-referrer'
            }
            return config
        }
    },
    paths: function (paths, env) {
        return paths;
    },
}