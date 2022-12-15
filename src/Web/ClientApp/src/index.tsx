import * as React from 'react';
import {createRoot} from 'react-dom/client';
import {Provider} from 'react-redux';
import { BrowserRouter } from "react-router-dom";
import {store} from './store';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import {CssBaseline} from "@mui/material";

// Get the application-wide store instance, prepopulating with state from the server where available.
const container = document.getElementById('root');
const root = createRoot(container!);
root.render(
    <Provider store={store}>
        <CssBaseline/>
        <BrowserRouter>
            <App/>
        </BrowserRouter>
    </Provider>);

registerServiceWorker();
