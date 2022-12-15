/*
 * Copyright 2012 Swail, Arnaud Ménard
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
namespace H3Standard
{
    public enum H3ErrorCodes
    {
        E_SUCCESS = 0,  // Success (no error)
        E_FAILED = 1,  // The operation failed but a more specific error is not available
        E_DOMAIN = 2,  // Argument was outside of acceptable range (when a more
                       // specific error code is not available)
        E_LATLNG_DOMAIN = 3,  // Latitude or longitude arguments were outside of acceptable range
        E_RES_DOMAIN = 4,  // Resolution argument was outside of acceptable range
        E_CELL_INVALID = 5,  // `H3Index` cell argument was not valid
        E_DIR_EDGE_INVALID = 6,  // `H3Index` directed edge argument was not valid
        E_UNDIR_EDGE_INVALID = 7,  // `H3Index` undirected edge argument was not valid
        E_VERTEX_INVALID = 8,  // `H3Index` vertex argument was not valid
        E_PENTAGON = 9,  // Pentagon distortion was encountered which the algorithm
                         // could not handle it
        E_DUPLICATE_INPUT = 10, // Duplicate input was encountered in the arguments
                                // and the algorithm could not handle it
        E_NOT_NEIGHBORS = 11, // `H3Index` cell arguments were not neighbors
        E_RES_MISMATCH = 12, // `H3Index` cell arguments had incompatible resolutions
        E_MEMORY_ALLOC = 13, // Necessary memory allocation failed
        E_MEMORY_BOUNDS = 14, // Bounds of provided memory were not large enough
        E_OPTION_INVALID = 15  // Mode or flags argument was not valid.
    }
}


