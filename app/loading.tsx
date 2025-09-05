import { ChefHat } from 'lucide-react'

export default function Loading() {
  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="text-center">
        <ChefHat className="w-16 h-16 mx-auto mb-4 text-primary animate-pulse" />
        <p className="text-gray-600">Loading...</p>
      </div>
    </div>
  )
}