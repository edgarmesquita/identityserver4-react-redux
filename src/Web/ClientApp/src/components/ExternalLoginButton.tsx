import * as React from 'react';
import {
    Link as RouterLink,
} from 'react-router-dom';
import {Button, useTheme} from "@mui/material";
import {FaFacebook} from "react-icons/fa";
import {FcGoogle} from "react-icons/fc";

interface IExternalLoginButtonProps {
    scheme: string;
    displayName: string;
    returnUrl?: string;
}

const schemes = [
    {name: "Google", color: "#FFF", icon: FcGoogle},
    {name: "Facebook", color: "", icon: FaFacebook}
]
const ExternalLoginButton = ({scheme, displayName, returnUrl}: IExternalLoginButtonProps) => {
    const schemeInfo = schemes.find(o => o.name === scheme)!;
    const theme = useTheme();
    let url = `/external/challenge?scheme=${scheme}`;
    if(returnUrl)
    {
        url += `&returnUrl=${encodeURIComponent(returnUrl)}`;
    }
    return (
        <Button startIcon={<schemeInfo.icon/>}
                href={url}
                type="submit"
                variant="contained"
                size="large"
                fullWidth sx={{
                    mt: 2,
                    backgroundColor: schemeInfo.color,
                    borderColor: '#CCC',
                    color: theme.palette.text.primary,
                    '&:hover': {
                        backgroundColor: '#DDD',
                        borderColor: '#BBB',
                    },
                    '&:active': {
                        backgroundColor: '#DDD',
                        borderColor: '#BBB',
                    },
                }}>
            {displayName}
        </Button>
    );
}

export default ExternalLoginButton;