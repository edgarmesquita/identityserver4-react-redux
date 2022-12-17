import {useLocation} from "react-router-dom";
import * as React from "react";

export function useQuery() {
    const { search } = useLocation();

    return React.useMemo(() => new URLSearchParams(search), [search]);
}