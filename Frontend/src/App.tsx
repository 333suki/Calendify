import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ThemeProvider } from "./components/Settings/ThemeContext";

import Home from "./components/Home/Home.tsx";
import Login from "./components/Login/Login.tsx";
import ProtectedRoutes from "./utils/ProtectedRoutes";
import AdminRoutes from "./utils/AdminRoutes";
import Register from "./components/Register/Register.tsx";
import ForgotPassword from "./components/Login/ForgotPassword";
import ResetPassword from "./components/Login/ResetPassword";

import Events from "./components/Events/Events.tsx";
import EventDetail from "./components/Events/EventDetail.tsx";
import AdminEvents from "./components/AdminEvents/AdminEvents.tsx";
import Profile from "./components/Profile/Profile.tsx";
import Settings from "./components/Settings/Settings.tsx";
import RoomBookings from "./components/RoomBookings/RoomBookings.tsx";
import OfficeAttendance from "./components/OfficeAttendance/OfficeAttendance.tsx";
import AdminRooms from "./components/AdminRooms/AdminRooms.tsx";

function App() {
    return (
        <ThemeProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<Login/>}/>
                    <Route path="/register" element={<Register/>}/>
                    <Route path="/forgot-password" element={<ForgotPassword/>}/>
                    <Route path="/reset-password" element={<ResetPassword/>}/>
                    <Route element={<ProtectedRoutes/>}>
                        <Route path="/home" element={<Home/>} />
                        <Route path="/room-bookings" element={<RoomBookings />} />
                        <Route path="/office-attendance" element={<OfficeAttendance />} />
                        <Route path="/events" element={<Events />} /> 
                        <Route path="/events/:eventId" element={<EventDetail />}/>
                        <Route path="/profile" element={<Profile />} />
                        <Route path="/settings" element={<Settings />} />
                    </Route>
                    <Route element={<AdminRoutes/>}>
                        <Route path="/admin/events" element={<AdminEvents/>} />
                        <Route path="/admin/rooms" element={<AdminRooms/>} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </ThemeProvider>
    )
}

export default App;
