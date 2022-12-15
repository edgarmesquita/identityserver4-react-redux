import * as React from 'react';
import { useAppSelector, useAppDispatch } from '../hooks/store'
import { increment } from '../store/counter'
const Counter = () => {
    // The `state` arg is correctly typed as `RootState` already
    const count = useAppSelector((state) => state.counter.value)
    const dispatch = useAppDispatch()
    
    return (
        <React.Fragment>
            <h1>Counter</h1>

            <p>This is a simple example of a React component.</p>

            <p aria-live="polite">Current count: <strong>{count}</strong></p>

            <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={() => { dispatch(increment()); }}>
                Increment
            </button>
        </React.Fragment>
    );
}

export default Counter;
