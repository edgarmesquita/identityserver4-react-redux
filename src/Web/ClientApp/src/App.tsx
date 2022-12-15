import * as React from 'react';
import { Route, Routes } from "react-router";
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';

export default () => (
    <Layout>
        <Routes>
            <Route path='/' element={<Home/>}/>
            <Route path='/counter' element={<Counter/>}/>
        </Routes>
    </Layout>
);
