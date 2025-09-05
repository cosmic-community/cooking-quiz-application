'use client'

import Link from 'next/link'
import { usePathname } from 'next/navigation'
import type { Category } from '@/types'

interface CategoryFilterProps {
  categories: Category[]
  selectedCategory: string | null
}

export default function CategoryFilter({ categories, selectedCategory }: CategoryFilterProps) {
  const pathname = usePathname()
  const isQuizzesPage = pathname === '/quizzes'

  if (!categories || categories.length === 0) {
    return null
  }

  const getCategoryStyle = (isSelected: boolean) => {
    if (isSelected) {
      return 'bg-primary text-white'
    }
    return 'bg-white text-gray-700 hover:bg-gray-100 border border-gray-300'
  }

  if (!isQuizzesPage) {
    // On homepage, just show category links
    return (
      <div className="flex flex-wrap gap-3 justify-center">
        <Link
          href="/quizzes"
          className="px-4 py-2 rounded-full font-medium transition-colors bg-white text-gray-700 hover:bg-gray-100 border border-gray-300"
        >
          All Categories
        </Link>
        {categories.map((category) => (
          <Link
            key={category.id}
            href={`/quizzes?category=${category.slug}`}
            className="px-4 py-2 rounded-full font-medium transition-colors bg-white text-gray-700 hover:bg-gray-100 border border-gray-300"
            style={{
              borderColor: category.metadata?.color || '#e5e7eb',
              color: category.metadata?.color || '#374151'
            }}
          >
            {category.metadata?.icon && (
              <span className="mr-2">{category.metadata.icon}</span>
            )}
            {category.title}
          </Link>
        ))}
      </div>
    )
  }

  // On quizzes page, show filter with selected state
  return (
    <div className="flex flex-wrap gap-3">
      <Link
        href="/quizzes"
        className={`px-4 py-2 rounded-full font-medium transition-colors ${getCategoryStyle(!selectedCategory)}`}
      >
        All Categories
      </Link>
      {categories.map((category) => {
        const isSelected = selectedCategory === category.slug
        return (
          <Link
            key={category.id}
            href={`/quizzes?category=${category.slug}`}
            className={`px-4 py-2 rounded-full font-medium transition-colors ${getCategoryStyle(isSelected)}`}
          >
            {category.metadata?.icon && (
              <span className="mr-2">{category.metadata.icon}</span>
            )}
            {category.title}
          </Link>
        )
      })}
    </div>
  )
}