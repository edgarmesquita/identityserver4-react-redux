import * as React from 'react';
import {
    Link as RouterLink,
} from 'react-router-dom';
import {AppBar, Box, Button, SvgIcon, Toolbar, Typography} from "@mui/material";
import IconButton from '@mui/material/IconButton';
import {MdMenu} from "react-icons/md";
const NavMenu = () => {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static">
                <Toolbar>
                    <IconButton
                        size="large"
                        edge="start"
                        color="inherit"
                        aria-label="menu"
                        sx={{ mr: 2 }}
                    >
                        <SvgIcon sx={{ width: 24, height: 24, color: 'white' }} component={MdMenu} inheritViewBox />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        IdentityServer 4 With React
                    </Typography>

                        <Button component={RouterLink} to={"/"} color="inherit">Home</Button>
                        <Button component={RouterLink} to={"/counter"} color="inherit">Counter</Button>

                </Toolbar>
            </AppBar>
        </Box>
    );
}
export default NavMenu;
