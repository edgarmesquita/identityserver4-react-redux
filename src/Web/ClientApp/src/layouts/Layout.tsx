import * as React from 'react';
import {ReactNode} from "react";
import {Box, Card, Container, useTheme} from "@mui/material";

interface ILayoutProps {
    children?: ReactNode;
}

const Layout: React.FC<ILayoutProps> = ({children}: ILayoutProps) => {
    const theme = useTheme();
    return (
        <Box sx={{
            backgroundImage: 'url(/images/home-bg.png)',
            backgroundPosition: 'center center',
            backgroundRepeat: 'no-repeat',
            backgroundSize: 'cover', color: '#FFFFFF',
            backgroundColor: theme.palette.grey[100],
            minHeight: '100%', flexGrow: 1, display: 'flex', flexDirection: 'column', py: 6
        }}>
            <Container maxWidth="xs">
                <Card sx={{
                    display: 'flex',
                    overflow: 'visible',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center'
                }}>
                    {children}
                </Card>
            </Container>
        </Box>
    );
}
export default Layout;