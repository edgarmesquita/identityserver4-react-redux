import * as React from 'react';
import {Button, useTheme} from "@mui/material";
import {FaFacebook} from "react-icons/fa";
import {FcGoogle} from "react-icons/fc";
import {darken} from "@mui/material";
interface IExternalLoginButtonProps {
    scheme: string;
    displayName: string;
    returnUrl?: string;
}

const schemes = [
    {name: "Google", color: "#FFF", icon: FcGoogle},
    {name: "Facebook", color: "#4267B2", icon: FaFacebook}
]
const ExternalLoginButton = ({scheme, displayName, returnUrl}: IExternalLoginButtonProps) => {
    const schemeInfo = schemes.find(o => o.name === scheme)!;
    const theme = useTheme();
    let url = `/external/challenge?scheme=${scheme}`;
    if (returnUrl) {
        url += `&returnUrl=${encodeURIComponent(returnUrl)}`;
    }
    const bgColorHover = darken(schemeInfo.color, .1);
    const borderColorHover = darken(schemeInfo.color, .2);
    return (
        <Button startIcon={<schemeInfo.icon/>}
                href={url}
                type="submit"
                variant="contained"
                size="large"
                fullWidth sx={{
                    mt: 2,
                    backgroundColor: schemeInfo.color,
                    borderColor: bgColorHover,
                    color: theme.palette.getContrastText(schemeInfo.color),
                    '&:hover': {
                        backgroundColor: bgColorHover,
                        borderColor: borderColorHover,
                    },
                    '&:active': {
                        backgroundColor: bgColorHover,
                        borderColor: borderColorHover,
                    },
                }}>
            Sign In with {displayName}
        </Button>
    );
}

export default ExternalLoginButton;