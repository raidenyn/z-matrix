export interface Molecule {
    atoms: Atom[]
    
    bonds: Bond[]
}

export interface Atom {
    element: string
    
    point: Point
    
    color: Color
}

export interface Point {
    x: number

    y: number

    z: number
}

export interface Color {
    r: number
    
    g: number
    
    b: number
}

export interface Bond {
    atom1: number

    atom2: number
}