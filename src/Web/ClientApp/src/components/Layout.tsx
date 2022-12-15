import * as React from 'react';
import NavMenu from './NavMenu';
import {ReactNode} from "react";
import {Container} from "@mui/material";

interface ILayoutProps {
    children?: ReactNode;
}
const Layout : React.FC<ILayoutProps> = ({children}: ILayoutProps) => {
    return (
        <React.Fragment>
            <NavMenu />
            <Container>
                {children}
            </Container>
        </React.Fragment>
    );
}
export default Layout;