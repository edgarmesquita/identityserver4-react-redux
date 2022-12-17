import * as React from 'react';
import {Route, Routes} from "react-router";
import Home from './pages/Home';
import Counter from './pages/Counter';
import Login from "./pages/Login";
import Redirect from "./pages/Redirect";

export default () => (
    <Routes>
        <Route path='/' element={<Home/>}/>
        <Route path='/login' element={<Login/>}/>
        <Route path='/counter' element={<Counter/>}/>
        <Route path='/redirect' element={<Redirect/>}/>
    </Routes>
);
